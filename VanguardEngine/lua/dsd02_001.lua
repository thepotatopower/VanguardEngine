-- Diabolos, "Violence" Bruce

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.FL, FL.PlayerFrontLeft, q.FL, FL.PlayerFrontRight, q.Other, o.Resting
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRidePhase, t.Auto, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.OnAttack, t.Auto, p.HasPrompt, p.SB, 5
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() and obj.IsPlayerTurn() then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and obj.IsAttackingUnit() and obj.InFinalRush() then
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
		obj.FinalRush()
	elseif n == 2 then
		obj.Stand(1)
	end
	return 0
end