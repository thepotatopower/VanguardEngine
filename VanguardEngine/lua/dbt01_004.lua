-- Diabolos Boys, Eden

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, p.IsMandatory
	elseif n == 2 then
		return a.OnAttackHits, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.IsAttackingUnit() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		if obj.CanRetire(2) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		if obj.InFinalRush() then
			obj.SetAbilityPower(1, 5000)
			if obj.StoodByCardEffectThisTurn() then
				obj.SetAbilityCritical(1, 1)
			end
		end
	elseif n == 2 then
		obj.ChooseRetire(2)
	end
	return 0
end