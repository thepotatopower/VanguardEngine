-- Diabolos Jetbacker, Lenard

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
		return q.Location, l.Soul, q.Count, 1, q.Min, 0
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, t.Cont, p.IsMandatory
	elseif n == 2 then
		return a.OnAttackHits, t.Auto, p.HasPrompt, p.IsMandatory
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
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		if obj.InFinalRush() then
			obj.SetAbilityPower(1, 5000)
			obj.AllowColumnAttack(1)
		end
	elseif n == 2 then
		obj.SoulCharge(1) 
		if obj.VanguardIs("Diabolos, \"Violence\" Bruce") and obj.NumPlayerOpenCircles() > 0 then
			obj.SuperiorCall(2, FL.OpenCircle)
		end
	end
	return 0
end
