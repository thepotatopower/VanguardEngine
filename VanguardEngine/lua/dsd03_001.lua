-- Apex Ruler, Bastion

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Grade, 3
	elseif n == 2 then
		return q.Location, l.RevealedDriveChecks, q.Grade, 3, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Count, 1
	elseif n == 4 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, p.IsMandatory
	elseif n == 2 then
		return a.OnBattleEnds, p.HasPrompt, p.OncePerTurn, p.Discard, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and obj.Exists(2) then
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

function Activate(n)
	if n == 1 then
		if obj.IsPlayerTurn() then
			obj.SetAbilityPower(1, 2000)
		end
	elseif n == 2 then
		obj.Select(3)
		obj.Stand(4)
		obj.AddTempPower(4, 10000)
		obj.EndSelect()
	end
	return 0
end