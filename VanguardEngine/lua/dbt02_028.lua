-- Berserk Dragon

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 3 then
		return q.Location, l.EnemyRC, q.Grade, 2, q.Other, o.GradeOrLess, q.Other, o.CanChoose, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerVC, q.NameContains, "Overlord", q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, t.Auto, p.HasPrompt, false, p.IsMandatory, true
	elseif n == 2 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.Exists(4) then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.Exists(4) and obj.CanCB(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		if obj.Exists(3) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 2 then
		obj.CounterBlast(2)
	end
end

function Activate(n)
	if n == 1 then 
		if obj.Exists(4) then
			obj.AddBattleOnlyPower(1, 5000)
		end
	elseif n == 2 then
		obj.ChooseRetire(3)
	end
	return 0
end