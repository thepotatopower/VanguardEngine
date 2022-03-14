-- モモッケ

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetCost("ACTCost")
	ability1.SetActivation("ACT")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.RC)
	ability2.SetProperty(p.Glitter)
	ability2.SetCondition("ContCondition")
	ability2.SetActivation("Cont")
end

function ACTCost(check)
	if check then return obj.CanRetire({q.Location, l.PlayerRC, q.Other, o.NotThis, q.Count, 1}) end
	obj.ChooseRetire({q.Location, l.PlayerRC, q.Other, o.NotThis, q.Min, 1})
end

function ACT()
	local power = 5000 * obj.GetNumberOf({q.Location, l.SuccessfullyRetired})
	obj.AddCardValue({q.Other, o.This}, cs.BonusPower, power, p.UntilEndOfTurn)
end

function ContCondition()
	return obj.GetNumberOf("Filter", {l.Units}) > 0
end

function Filter(id)
	return obj.IsPlayer(id) and obj.IsVanguard(id) and obj.NameContains(id, "Rorowa") and obj.HasProperty(id, p.IsGlitter)
end

function Cont()
	if obj.Exists({q.Location, l.PlayerUnits, q.NameContains, obj.LoadName("Radylina"), q.Other, o.SameColumn}) then
		obj.AddCardState({q.Other, o.This}, cs.CanAttackFromBackRow, p.Continuous)
	end
end
