-- Aim to be the Strongest Idol!

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerUnits, q.NameContains, "Earnescorrect", q.Other, o.DifferentNames, q.Count, 5
	elseif n == 2 then
		return q.Location, l.PlayerUnits, q.NameContains, "Earnescorrect"
	elseif n == 3 then
		return q.Location, l.EnemyVC, q.Grade, 3, q.Other, o.GradeOrHigher, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerVC, q.Name, "Earnescorrect Leader, Clarissa", q.Count, 1
	elseif n == 5 then
		return q.Location, l.PlayerVC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, p.HasPrompt
	elseif n == 2 then
		return a.Cont, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(1) then 
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		return true
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.AddTempPower(2, 5000)
		if obj.Exists(3) then
			obj.ChooseGiveAbility(4, 2)
		end
	elseif n == 2 then
		obj.AddContinuousState(5, cs.CanChooseThreeCirclesWhenAttacking)
	end
	return 0
end