-- 樹角獣 ガボレーグ

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.PlayerRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.Attacking, q.Other, o.SameColumn, q.Other, o.NotThis})
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function CanFullyResolve()
	return obj.IsSameZone()
end

function Activation()
	if obj.IsSameZone() then
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusPower, 10000, p.UntilEndOfTurn)
		local selected = obj.AutoSelect({q.Location, l.EnemyRC, q.Other, o.CanChoose})
		local ability2 = NewAbility(GetID())
		ability2.SetTiming(a.Cont)
		ability2.SetLocation(l.PlayerUnits)
		ability2.SetActivation("TimedActivation")
		ability2.SetResetTiming(p.UntilEndOfTurn)
		ability2.SetResetTarget(GetID())
		ability2.Store(selected)
	end
end

function TimedActivation()
	if obj.IsAttackingUnit() then
		local grades = obj.GetGrades({q.Location, l.Stored})
		for key,value in pairs(grades) do
			obj.AddPlayerValue(ps.CannotCallGradeToGC, value, p.UntilEndOfBattle, false)
		end
	end
end