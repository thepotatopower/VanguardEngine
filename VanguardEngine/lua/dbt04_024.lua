-- 極光戦姫 サプレス・グリーマ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Cont")
	-- on place
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.PlacedOnRC)
	ability2.SetMovedFrom(l.Hand)
	ability2.SetTrigger("OnPlaceTrigger")
	ability2.SetCondition("OnPlaceCondition")
	ability2.SetActivation("OnPlace")
end

function Cont()
	if obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Seraph")}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.Exists({q.Location, l.PlayerPrisoners, q.Count, 3}) 
end

function OnPlace()
	obj.SuperiorCall({q.Location, l.Deck, q.Name, "ThisUnitName", q.Count, 1})
	obj.Shuffle()
end