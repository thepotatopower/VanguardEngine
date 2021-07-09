-- Diabolos, "Violence" Bruce

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 5
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.FL, FL.PlayerFrontLeft, q.FL, FL.PlayerFrontRight, q.Other, o.Resting
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRidePhase, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	elseif n == 2 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and obj.IsAttackingUnit() and obj.InFinalRush() and obj.CanSB(1) then
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

function Cost(n)
	if n == 2 then
		obj.SoulBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.FinalRush()
	elseif n == 2 then
		obj.Stand(2)
	end
	return 0
end