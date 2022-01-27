-- 封焔竜 アーダルラ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCondition("OnPlaceCondition")
	ability1.SetCost("OnPlaceCost")
	ability1.SetCanFullyResolve("OnPlaceCanFullyResolve")
	ability1.SetActivation("OnPlace")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.RC)
	ability2.SetActivation("Cont")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.GetArmsCount() == 1
end

function OnPlaceCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnPlaceCanFullyResolve()
	return obj.Exists({q.Location, l.Hand, q.Location, l.Drop, q.Other, o.Arms, q.Count, 1})
end

function OnPlace()
	obj.Select({q.Location, l.Hand, q.Location, l.Drop, q.Other, o.Arms, q.Count, 1})
	obj.Arm({q.Location, l.Selected})
end

function Cont()
	if obj.IsPlayerTurn() and obj.GetArmsCount() >= 2 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end
