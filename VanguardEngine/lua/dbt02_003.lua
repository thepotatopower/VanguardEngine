-- Crimson Expeller

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Other, o.This, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerVC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.OnACT, p.HasPrompt, p.AddToDrop, 1, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.TargetIsEnemyVanguard() then
			return true
		end
	elseif n == 2 then
		if obj.Exists(1) then
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
		obj.SoulCharge(1)
	elseif n == 2 then
		obj.SoulCharge(1) 
		obj.ChooseAddTempPower(3, 10000)
	end
	return 0
end
