-- グラビディア・バリンジャー

function RegisterAbilities()
	-- placed
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("PlacedTrigger")
	ability1.SetCondition("PlacedCondition")
	ability1.SetCost("PlacedCost")
	ability1.SetActivation("Placed")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.PlayerRC)
	ability2.SetActivation("Cont")
end

function PlacedTrigger()
	return obj.IsApplicable()
end

function PlacedCondition()
	return obj.Exists({q.Location, l.Order, q.Other, o.Meteorite})
end

function PlacedCost(check)
	if check then return obj.CanSB(3) end
	obj.SoulBlast(3)
end

function Placed()
	obj.Draw(1)
	if obj.IsSameZone() then
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	end
end

function Cont()
	if obj.Exists({q.Location, l.Drop, q.Location, l.Order, q.Other, o.SetOrder, q.Count, 5}) then
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end