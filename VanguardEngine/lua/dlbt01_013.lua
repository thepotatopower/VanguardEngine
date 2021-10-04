-- Brilliance and Elegance, Aerith

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Name, "Astesice, Kairi", q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerHand, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsBoosted() and obj.Exists(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		if obj.Exists(2) and obj.CanSuperiorCallToSpecificCircle(2, FL.OpenCircle) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.SuperiorCallToSpecificCircle(2, FL.OpenCircle)
	end
	return 0
end