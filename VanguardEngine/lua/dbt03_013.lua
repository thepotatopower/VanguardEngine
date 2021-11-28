-- ドラグリッター シハーブ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCondition("OnPlaceCondition")
	ability1.SetActivation("OnPlace")
	-- ride deck discard replace
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetPrompt(3)
	ability2.SetTiming(a.RideDeckDiscardReplace)
	ability2.SetLocation(l.PlayerRC)
	ability2.SetCost("RideDeckDiscardReplaceCost")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.IsSameZone()
end

function OnPlace()
	obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end

function RideDeckDiscardReplaceCost(check)
	if check then return obj.CanAddToSoul({q.Location, l.PlayerRC, q.Other, o.This}) end
	obj.AddToSoul({q.Location, l.PlayerRC, q.Other, o.This})
end