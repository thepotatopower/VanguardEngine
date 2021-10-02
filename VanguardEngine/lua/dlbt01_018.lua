-- Quiet Love, Elivira

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Applicable, q.Name, "Everlasting Sapphire", q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerVC, q.Name, "Prismajica, Wilista", q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrderPlayed, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.Exists(1) and obj.Exists(2) then
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
		obj.Draw(1)
	end
	return 0
end