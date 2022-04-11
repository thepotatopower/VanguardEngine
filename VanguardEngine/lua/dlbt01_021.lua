-- 目指せ！最強のアイドル！

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Condition()
	return obj.Exists({q.Location, l.PlayerUnits, q.NameContains, obj.LoadName("Earnescorrect"), q.Count, 5, q.Other, o.DifferentNames})
end

function Activation()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.NameContains, obj.LoadName("Earnescorrect")}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	if obj.Exists({q.Location, l.EnemyVC, q.Grade, 3, q.Other, o.GradeOrHigher}) then
		obj.Select({q.Location, l.PlayerUnits, q.Name, obj.GetNameFromCardID("dlbt01_001"), q.Count, 1})
		local id = obj.GetID({q.Location, l.Selected})
		if id >= 0 then
			local given = GiveAbility(GetID(), id)
			given.SetDescription(2)
			given.SetTiming(a.Cont)
			given.SetLocation(l.VC)
			given.SetActivation("Cont")
			given.SetResetTarget(id)
			given.SetResetTiming(p.UntilEndOfTurn)
		end
	end
end

function Cont()
	obj.AddCardState({q.Other, o.This}, cs.CanChooseThreeCirclesWhenAttacking, p.Continuous)
end