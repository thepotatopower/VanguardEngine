-- Downpouring Singer, Elkiel

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
		return q.Location, l.Applicable, q.Other, o.Player, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnVC, p.HasPrompt, p.Bind, 1
	elseif n == 2 then
		return a.OnBind, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.Exists(2) then
			return true
		end
	elseif n == 2 then
		if obj.IsApplicable() then
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
		obj.Draw(1)
	elseif n == 2 then
		obj.AddUntilEndOfTurnPlayerState(ps.BlackAndWhiteWingsActive)
	end
	return 0
end