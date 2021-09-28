-- Berserk Dragon

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.EnemyRC, q.Grade, 2, q.Other, o.GradeOrLess, q.Other, o.CanChoose, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerVC, q.NameContains, "Overlord", q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.Exists(3) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then 
		if obj.Exists(3) then
			obj.AddBattleOnlyPower(1, 5000)
		end
		if obj.CanCB(1) and obj.YesNo("Counter Blast 1 to retire opponent's grade 2 or less rear-guard?") then
			obj.CounterBlast(1)
			obj.ChooseRetire(2)
		end
	end
	return 0
end