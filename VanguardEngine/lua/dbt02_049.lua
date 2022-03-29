-- Form Up, O Chosen Knights

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Grade, 3, q.Count, 2
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Grade, 3, q.Count, 3
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Grade, 3, q.Count, 4
	elseif n == 4 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Grade, 3, q.Count, 1
	elseif n == 5 then
		return q.Location, l.PlayerVC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, p.HasPrompt, p.CB, 1, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
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
		if obj.Exists(1) then
			obj.ChooseAddTempPower(4, 5000)
		end
		if obj.Exists(2) then
			obj.Draw(1)
		end
		if obj.Exists(3) then
			obj.ChooseAddDrive(5, 1)
		end
	end
	return 0
end