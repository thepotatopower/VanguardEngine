-- 髑髏の魔女 ネヴァン

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
	ability2.SetTrigger("OnPlaceTrigger")
	ability2.SetCondition("OnPlaceCondition")
	ability2.SetCost("OnPlaceCost")
	ability2.SetActivation("OnPlace")
end

function Cont()
	if obj.IsPlayerTurn() and obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Blaster")}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.GetNumberOf("Filter", {l.Units}) < 1
end

function Filter(id)
	return obj.IsPlayer(id) and obj.IsVanguard(id) and not obj.IsClan(id, c.ShadowPaladin)
end

function OnPlaceCost(check)
	if check then return obj.CanCB(1) and obj.CanAddToSoul({q.Location, l.Hand, q.Count, 1}) end
	obj.CounterBlast(1)
	obj.ChooseAddToSoul({q.Location, l.Hand, q.Count, 1})
end

function OnPlace()
	obj.Draw(2)
end